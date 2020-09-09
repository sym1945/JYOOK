using JYOOK.Application;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace JYOOK
{
    public class PackingTemplateManageViewModel : ViewModelBase
    {
        private readonly PackResourceService _PackResourceService = PackResourceService.Instance;


        public ObservableCollection<PackingTemplateViewModel> PackingTemplates { get; set; } = new ObservableCollection<PackingTemplateViewModel>();

        public PackingTemplateViewModel SelectedTemplate { get; set; }

        public bool IsApplied { get; private set; }


        public ICommand LoadedCommand
        {
            get => new CommandBase
            {
                ExecuteAction = async (param) =>
                {
                    var packingTempaltes = await _PackResourceService.GetPackingTemplates();
                    foreach (var packingTemplate in packingTempaltes)
                    {
                        PackingTemplates.Add(new PackingTemplateViewModel
                        {
                            Id = packingTemplate.Id,
                            Name = packingTemplate.Name,
                            PackingWeight = packingTemplate.PackingWeight,
                            Resources = new ObservableCollection<PackResourceViewModel>(
                                packingTemplate.Resources.Select(d => new PackResourceViewModel
                                {
                                    Id = d.Resource.Id,
                                    Name = d.Resource.Name,
                                    Price = d.Resource.Price,
                                    Type = d.Resource.Type,
                                    Count = d.Count,
                                })
                            )
                        });
                    }
                }
            };
        }

        public ICommand SaveCommand
        {
            get => new CommandBase
            {
                ExecuteAction = (param) =>
                {
                    if (SelectedTemplate == null)
                    {
                        OnInfoMessage("적용할 템플릿을 선택해주세요.");
                        return;
                    }

                    if (OnRequestMessage("선택한 템플릿으로 적용하시겠습니까?"))
                    {
                        IsApplied = true;
                        OnCloseRequest();
                    }

                }
            };
        }

        public ICommand RemoveCommand
        {
            get => new CommandBase
            {
                ExecuteAction = async (param) =>
                {
                    if (SelectedTemplate == null)
                    {
                        OnInfoMessage("삭제할 템플릿을 선택해주세요.");
                        return;
                    }

                    if (OnRequestMessage("선택한 템플릿을 삭제하시겠습니까?"))
                    {
                        await _PackResourceService.RemovePackingTemplate(SelectedTemplate);

                        PackingTemplates.Remove(SelectedTemplate);
                    }

                }
            };
        }

        public PackingTemplateManageViewModel()
        {
        }

    }
}