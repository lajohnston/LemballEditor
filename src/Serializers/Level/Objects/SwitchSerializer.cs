//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LemballEditor.Models;
//using LemballEditor.Models.GameObjects;

//namespace LemballEditor.Serializers.Level.Objects
//{
//    public class SwitchSerializer : ISerializer<(Switch, Position, uint)>
//    {
//        public (Switch, Position, uint) Deserialize(BinaryReader reader, (Switch, Position, uint) models)
//        {
//            var (_, position, _) = models;
//            var model = new Switch(position); // todo: use a factory


//            throw new NotImplementedException();
//        }

//        public void Serialize((Switch, Position, uint) model, BinaryWriter writer)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
