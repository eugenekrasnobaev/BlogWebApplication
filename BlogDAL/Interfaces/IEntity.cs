using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDAL.Interfaces
{
    public interface IEntity
    {
        int Id { get; }

        bool IsActive { get; set; }
    }
}
