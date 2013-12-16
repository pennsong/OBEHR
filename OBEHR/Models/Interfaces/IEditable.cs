using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OBEHR.Models.Interfaces
{
    public interface IEditable<Model>
    {
        void Edit(Model model);
    }
}
