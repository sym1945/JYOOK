using JYOOK.Domain;
using System;
using System.Windows.Input;

namespace JYOOK.Application
{
    public class PackResourceViewModel : ViewModelBase
    {
        public bool IsSelected { get; set; }

        public Guid Id { get; set; }

        public ResourceType? Type { get; set; }

        public string Name { get; set; }

        public double? Price { get; set; }

        public ushort Count { get; set; } = 1;

        public double? TotalPrice => Price * Count;

        public ICommand RemoveCommand
        {
            get => new CommandBase
            {
                ExecuteAction = (param) =>
                {
                    RemoveRequest?.Invoke(this);
                }
            };
        }

        public event Action<PackResourceViewModel> RemoveRequest;


        public PackResourceViewModel()
        {
        }

        public PackResourceViewModel(ExtraResource resource)
        {
            Id = resource.Id;
            Type = resource.Type;
            Name = resource.Name;
            Price = resource.Price;
        }

        public PackResourceViewModel Clone()
        {
            return new PackResourceViewModel
            {
                Id = Id,
                Type = Type,
                Name = Name,
                Price = Price,
                Count = Count
            };
        }


        public ExtraResource ToResource()
        {
            return new ExtraResource
            {
                Id = Id,
                Type = (ResourceType)Type,
                Name = Name,
                Price = (double)Price
            };
        }

    }
}