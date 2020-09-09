using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JYOOK.Application
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<object, string> InfoMessage;

        public event Action<object, string> ErrorMessage;

        public event Func<object, string, bool> RequestMessage;

        public event Action<object> CloseRequest;


        protected void OnErrorMessage(string message)
        {
            ErrorMessage?.Invoke(this, message);
        }

        protected void OnInfoMessage(string message)
        {
            InfoMessage?.Invoke(this, message);
        }

        protected bool OnRequestMessage(string message)
        {
            return RequestMessage?.Invoke(this, message) ?? false;
        }

        protected void OnCloseRequest()
        {
            CloseRequest?.Invoke(this);
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}