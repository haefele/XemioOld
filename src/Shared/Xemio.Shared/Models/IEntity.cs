using System;
using System.Collections.Generic;
using System.Text;

namespace Xemio.Shared.Models
{
    interface IEntity
    {
        Guid Id { get; set; }
    }
}
