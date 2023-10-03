using Arlanet.Umbraco.Grid.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.Blocks;

namespace GMPA.Core.Models.ViewModels
{
    public class CaseViewModel : MainViewModel, IGridViewModel
    {
        public BlockListGrid BlockGrid { get; set; }
    }
}
