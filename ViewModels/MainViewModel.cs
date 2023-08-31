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
        public ReactiveCommand<Unit, Unit> NavigateToAlphCodingPageCommand { get; }
        #endregion

        public MainViewModel() 
        {
            AlphabeticCodingPage = new AlphabeticCodingPage();
            CurrentPage = AlphabeticCodingPage;
            NavigateToAlphCodingPageCommand = NavigateToAlphCodingPageCommand = ReactiveCommand.CreateFromObservable(NavigateToAlphCoding);
        }

        public IObservable<Unit> NavigateToAlphCoding()
        {
            CurrentPage = AlphabeticCodingPage;
            return Observable.Return(Unit.Default);
        }
    }
}
