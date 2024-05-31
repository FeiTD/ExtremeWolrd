using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum Type
    {
        None,
        Functional,
        Task
    }

    public enum NpcFunction
    {
        None,
        InvokeShop,
        InvokeInsrance
    }
    public class NpcDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Type Type {  get; set; }
        public NpcFunction Function { get; set; }
        public int Param { get; set; }
    }
}
