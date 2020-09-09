using JYOOK.Domain;
using JYOOK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JYOOK.Application
{
    public class PackResourceService
    {
        // 임시
        public readonly static PackResourceService Instance = new PackResourceService();
        // 임시
        private readonly IResourceRepository _ResourceRepo = ResourceRepository.Instance;
        // 임시
        private readonly IPackingTemplateRepository _PackingTempalteRepo = new PackingTemplateRepository();


        private List<ExtraResource> _Resources;

        private List<PackingTemplate> _PackingTemplates;

        public event Action<object, IEnumerable<ExtraResource>> ResourceModified;

        public event Action<object, IEnumerable<ExtraResource>> ResourceRemoved;


        public PackResourceService()
        {

        }


        public async ValueTask<List<ExtraResource>> GetResources()
        {
            if (_Resources == null)
            {
                _Resources = await _ResourceRepo.GetResources();
            }

            await GetPackingTemplates();

            return _Resources;
        }

        public async Task SaveResources(IEnumerable<ExtraResource> saveItems)
        {
            var removedList = new List<ExtraResource>();
            var modifiedList = new List<ExtraResource>();
            var addedList = new List<ExtraResource>();
            var existIds = new List<Guid>();

            foreach (var saveItem in saveItems)
            {
                var resource = _Resources.FirstOrDefault(r => r.Id == saveItem.Id);
                if (resource == null)
                {
                    // 추가 대상
                    addedList.Add(saveItem);
                }
                else
                {
                    existIds.Add(saveItem.Id);

                    // 수정 대상
                    if (resource.Name != saveItem.Name
                        || resource.Type != saveItem.Type
                        || resource.Price != saveItem.Price)
                    {
                        // 수정
                        resource.Name = saveItem.Name;
                        resource.Type = (ResourceType)saveItem.Type;
                        resource.Price = (double)saveItem.Price;

                        modifiedList.Add(resource);
                    }
                }
            }

            // 삭제 대상
            removedList.AddRange(_Resources.Where(r => existIds.Contains(r.Id) == false));

            // 추가
            foreach (var addItem in addedList)
            {
                addItem.Id = Guid.NewGuid();
                _Resources.Add(addItem);
            }

            // 삭제
            _Resources.RemoveAll(r => removedList.Contains(r));

            await _ResourceRepo.SaveResources(_Resources);

            if (addedList.Count > 0)
            {
                Debug.WriteLine($"추가: {string.Join(" ", addedList.Select(d => $"{d.Id}_{d.Name}"))}");
            }
            if (modifiedList.Count > 0)
            {
                Debug.WriteLine($"수정: {string.Join(" ", modifiedList.Select(d => $"{d.Id}_{d.Name}"))}");

                // modify to packing template
                if (_PackingTemplates != null && _PackingTemplates.Count > 0)
                {
                    foreach (var packingTemplate in _PackingTemplates)
                    {
                        foreach (var packResource in packingTemplate.Resources)
                        {
                            var modifiedResource = modifiedList.FirstOrDefault(d => d.Id.Equals(packResource.Resource.Id));
                            if (modifiedResource != null)
                            {
                                packResource.Resource.Name = modifiedResource.Name;
                                packResource.Resource.Price = modifiedResource.Price;
                                packResource.Resource.Type = modifiedResource.Type;
                            }
                        }
                    }

                    await _PackingTempalteRepo.Save(_PackingTemplates);
                }

                ResourceModified?.Invoke(this, modifiedList);
            }
            if (removedList.Count > 0)
            {
                Debug.WriteLine($"삭제: {string.Join(" ", removedList.Select(d => $"{d.Id}_{d.Name}"))}");

                // remove to packing template
                if (_PackingTemplates != null && _PackingTemplates.Count > 0)
                {
                    var removedResourceIds = removedList.Select(n => n.Id).ToList();
                    foreach (var packingTemplate in _PackingTemplates.ToList())
                    {
                        foreach (var packResource in packingTemplate.Resources.ToList())
                        {
                            if (removedResourceIds.Contains(packResource.Resource.Id))
                            {
                                packingTemplate.Resources.Remove(packResource);
                            }
                        }

                        if (packingTemplate.Resources.Count < 1)
                        {
                            _PackingTemplates.Remove(packingTemplate);
                        }
                    }

                    await _PackingTempalteRepo.Save(_PackingTemplates);
                }

                ResourceRemoved?.Invoke(this, removedList);
            }

        }


        public async ValueTask<List<PackingTemplate>> GetPackingTemplates()
        {
            if (_PackingTemplates == null)
            {
                _PackingTemplates = await _PackingTempalteRepo.GetPackinTemplates();
            }

            lock (_PackingTemplates)
            {
                return _PackingTemplates;
            }
        }

        public async Task<bool> CheckExistencePackingTemplateName(string templateName)
        {
            var templateNames = (await GetPackingTemplates()).Select(d => d.Name);
            return templateNames.Contains(templateName);
        }

        public async Task AddPackingTemplate(ResourceListViewModel newItem, string templateName)
        {
            var templateId = Guid.NewGuid();
            var newPackingTemplate = new PackingTemplate
            {
                Id = templateId,
                Name = templateName,
                PackingWeight = newItem.PackingWeight,
                Resources = newItem.ItemList.Select(d => new PackResourceTemplate
                {
                    PackingTemplateId = templateId,
                    Count = d.Count,
                    ExtraResourceId = d.Id,
                    Resource = new ExtraResource
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Price = d.Price.GetValueOrDefault(),
                        Type = d.Type.GetValueOrDefault()
                    }
                }).ToList()
            };

            lock (_PackingTemplates)
            {
                _PackingTemplates.Add(newPackingTemplate);
            }

            await _PackingTempalteRepo.Save(_PackingTemplates);
        }

        public async Task RemovePackingTemplate(PackingTemplateViewModel packingTemplate)
        {
            var targetItem = _PackingTemplates.FirstOrDefault(d => d.Id == packingTemplate.Id);
            if (targetItem == null)
                return;

            _PackingTemplates.Remove(targetItem);

            await _PackingTempalteRepo.Save(_PackingTemplates);
        }


        public async Task SavePackingTemplate(IEnumerable<PackingTemplate> saveItems)
        {
            if (_PackingTemplates == null)
                return;

            lock (_PackingTemplates)
            {
                _PackingTemplates.Clear();
                _PackingTemplates = saveItems.ToList();
            }

            await _PackingTempalteRepo.Save(_PackingTemplates);
        }


    }
}