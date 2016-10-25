//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Entitas;

namespace Entitas {

    public partial class Entity {

        public XMLComponent xML { get { return (XMLComponent)GetComponent(InputComponentIds.XML); } }
        public bool hasXML { get { return HasComponent(InputComponentIds.XML); } }

        public Entity AddXML(string newName) {
            var component = CreateComponent<XMLComponent>(InputComponentIds.XML);
            component.name = newName;
            return AddComponent(InputComponentIds.XML, component);
        }

        public Entity ReplaceXML(string newName) {
            var component = CreateComponent<XMLComponent>(InputComponentIds.XML);
            component.name = newName;
            ReplaceComponent(InputComponentIds.XML, component);
            return this;
        }

        public Entity RemoveXML() {
            return RemoveComponent(InputComponentIds.XML);
        }
    }
}

    public partial class InputMatcher {

        static IMatcher _matcherXML;

        public static IMatcher XML {
            get {
                if(_matcherXML == null) {
                    var matcher = (Matcher)Matcher.AllOf(InputComponentIds.XML);
                    matcher.componentNames = InputComponentIds.componentNames;
                    _matcherXML = matcher;
                }

                return _matcherXML;
            }
        }
    }
