using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CascadePass.Core.UI.Controls
{
    public class NavigationCommand : ICommand
    {
        private readonly NavigationItem _item;
        private readonly IEnumerable<NavigationItem> _allItems;
        private readonly Action<NavigationItem> _onSelected;

        public NavigationCommand(NavigationItem item,
                                 IEnumerable<NavigationItem> allItems,
                                 Action<NavigationItem> onSelected)
        {
            _item = item;
            _allItems = allItems;
            _onSelected = onSelected;
        }

        public IEnumerable<NavigationItem> Items { get; private set; }

        public bool CanExecute(object parameter)
        {
            return _item.Command?.CanExecute(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            // Deselect all other items in the collection
            foreach (var navItem in _allItems)
                navItem.IsSelected = navItem == _item;

            // Execute the original command
            _item.Command?.Execute(parameter ?? _item.CommandParameter);

            // Tell the world
            _onSelected?.Invoke(_item);
        }

        public event EventHandler CanExecuteChanged
        {
            add => _item.Command?.CanExecuteChanged += value;
            remove => _item.Command?.CanExecuteChanged -= value;
        }
    }
}
