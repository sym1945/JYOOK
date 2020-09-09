using JYOOK.Application;
using System.Windows.Input;

namespace JYOOK
{
    public class PackingTemplateAddViewModel : ViewModelBase
    {
        private readonly PackResourceService _PackResourceService = PackResourceService.Instance;

        private readonly ResourceListViewModel _ResourceList;

        public string TemplateName { get; set; }

        public ICommand AddCommand
        {
            get => new CommandBase
            {
                ExecuteAction = async (param) =>
                {
                    if (TemplateName.IsEmptyOrNullOrWhiteSpace())
                    {
                        OnErrorMessage("템플릿 이름을 입력해주세요.");
                        return;
                    }

                    if (await _PackResourceService.CheckExistencePackingTemplateName(TemplateName))
                    {
                        OnErrorMessage("존재하는 템플릿 이름입니다.");
                        return;
                    }

                    await _PackResourceService.AddPackingTemplate(_ResourceList, TemplateName);

                    OnCloseRequest();
                },
            };
        }

        public PackingTemplateAddViewModel(ResourceListViewModel resourceListViewModel)
        {
            _ResourceList = resourceListViewModel;
        }

    }
}