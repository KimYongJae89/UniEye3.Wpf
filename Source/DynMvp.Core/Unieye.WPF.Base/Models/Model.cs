using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.Models
{
    public abstract class Model
    {
        protected string _name;
        protected DateTime _modifiedDate;
        protected DateTime _registeredDate;
        protected string _description;

        public string Name => _name;
        public DateTime ModifiedDate => _modifiedDate;
        public DateTime RegisteredDate => _registeredDate;
        public string Description { get => _description; set => _description = value; }

        public Model(string name)
        {
            _name = name;
            _modifiedDate = DateTime.Now;
            _registeredDate = DateTime.Now;
        }
    }
}
