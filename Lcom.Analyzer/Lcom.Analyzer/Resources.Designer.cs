//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lcom.Analyzer {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Lcom.Analyzer.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Type lacks of cohesion.
        /// </summary>
        internal static string LcomId {
            get {
                return ResourceManager.GetString("LcomId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Type &apos;{0}&apos; Lcom is {1} (&lt; 0.8 expected).
        /// </summary>
        internal static string LcomMessageFormat {
            get {
                return ResourceManager.GetString("LcomMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Type lacks of cohesion.
        /// </summary>
        internal static string LcomTitle {
            get {
                return ResourceManager.GetString("LcomTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Type has too many fields.
        /// </summary>
        internal static string TooManyFieldsId {
            get {
                return ResourceManager.GetString("TooManyFieldsId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Type &apos;{0}&apos; counts {1} fields or auto-implemented properties (&lt;= 5 expected).
        /// </summary>
        internal static string TooManyFieldsMessageFormat {
            get {
                return ResourceManager.GetString("TooManyFieldsMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Type has too many fields.
        /// </summary>
        internal static string TooManyFieldsTitle {
            get {
                return ResourceManager.GetString("TooManyFieldsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Type has too many dynamic members.
        /// </summary>
        internal static string TooManyMethodsId {
            get {
                return ResourceManager.GetString("TooManyMethodsId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Type &apos;{0}&apos; counts {1} dynamic members (members executing statements) (&lt;= 20 expected).
        /// </summary>
        internal static string TooManyMethodsMessageFormat {
            get {
                return ResourceManager.GetString("TooManyMethodsMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Type has too many dynamic members.
        /// </summary>
        internal static string TooManyMethodsTitle {
            get {
                return ResourceManager.GetString("TooManyMethodsTitle", resourceCulture);
            }
        }
    }
}
