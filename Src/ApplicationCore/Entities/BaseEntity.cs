using System;
using System.Collections.Generic;
using System.Text;

namespace Kasanova.FaldoneFoto.ApplicationCore.Entities
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
    }

    public class NameEntity<T> : BaseEntity<T>
    {
        public virtual string Description { get; set; }
    }
}
