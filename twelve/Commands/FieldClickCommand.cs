using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using twelve.Commands.Base;
using twelve.Models;
using twelve.ViewModels;

namespace twelve.Commands
{
    public class FieldClickCommand : Command
    {
        public override void Execute(object parameters)
        {
            object[] param = parameters as object[];
            ((GameViewModel) param[1]).Click((FieldModel)param[0]);

        }
    }
}
