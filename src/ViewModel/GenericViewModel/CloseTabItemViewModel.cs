﻿using KarveCar.Commands.Generic;
using KarveCar.Logic.Generic;
using KarveCar.Model.Generic;
using System.Linq;
using System.Windows.Input;
using static KarveCar.Model.Generic.RecopilatorioCollections;
using static KarveCar.Model.Generic.RecopilatorioEnumerations;

namespace KarveCar.ViewModel.GenericViewModel
{
    public class CloseTabItemViewModel : GenericPropertyChanged
    {
        private CloseTabItemCommand closetabitemcommand;

        public CloseTabItemViewModel()
        {
            this.closetabitemcommand = new CloseTabItemCommand(this);
        }

        public ICommand CloseTabItemCommand
        {
            get
            {
                return closetabitemcommand;
            }
        }

        /// <summary>
        /// Cierra el TabItem según la EOpcion recibida por params
        /// </summary>
        /// <param name="parameter"></param>
        public void CloseTabItem(object parameter)
        {
            EOpcion tipoaux = ribbonbuttondictionary.Where(z => z.Key.ToString() == parameter.ToString()).FirstOrDefault().Key;
            TabItemLogic.RemoveTabItem(tipoaux);
        }
    }
}