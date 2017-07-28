﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;

namespace KarveCommon.Command
{
    public abstract class  AbstractCommand: ICommand

    {
        public event EventHandler CanExecuteChanged;
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }
        public abstract void Execute(object parameter);

        public virtual bool UnExecute()
        {
            throw new NotImplementedException();
        }

    }
}
