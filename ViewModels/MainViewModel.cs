using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LR_1.Views;
using System.Reactive.Linq;

namespace LR_1.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Pages
        private Page AlphabeticCodingPage;
        private Page HammingCodePage;
        #endregion

        #region Properties
        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }
        #endregion

        #region Commands
        public ReactiveCommand<string, Unit> NavigateToCommand { get; }
        #endregion

        public MainViewModel() 
        {
            AlphabeticCodingPage = new AlphabeticCodingPage();
            HammingCodePage = new HammingCodePage();
            CurrentPage = AlphabeticCodingPage;
            NavigateToCommand = ReactiveCommand.CreateFromObservable<string, Unit>(NavigateTo);
        }

        public IObservable<Unit> NavigateTo(string page)
        {
            switch (page)
            {
                case "AplhabeticCodingPage":
                    CurrentPage = AlphabeticCodingPage;
                    break;
                case "HammingCodePage":
                    CurrentPage = HammingCodePage;
                    break;
                default:
                    break;
            }
            
            return Observable.Return(Unit.Default);
        }
    }
}
