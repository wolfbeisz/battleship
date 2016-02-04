using System;
using System.IO;
using System.Collections.Generic;
using BattleSDK;
using System.Reflection;

namespace GameManager
{
	public static class KIs
	{
		public static List<Type> AvaliableKIs;

		public static void LoadKIs(){
			//Reset all avaliable KIs
			AvaliableKIs = new List<Type> ();

			/* The KIs will be loaded from this path.
			 * Currently there is a "Players" dir in the executables dir
			 */
			String path = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location) + "/Players";

			//Loads all .dll files in the specified folder
			string[] dllFileNames = null; 
			if(Directory.Exists(path)) 
			{ 
				dllFileNames = Directory.GetFiles(path, "*.dll"); 
			}

			//Loads all assemblies containing classes from the .dll files
			ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length); 
			foreach(string dllFile in dllFileNames) 
			{ 
				AssemblyName an = AssemblyName.GetAssemblyName(dllFile); 
				Assembly assembly = Assembly.Load(an); 
				assemblies.Add(assembly); 
			}

			//In each assembly
			foreach(Assembly assembly in assemblies) 
			{ 
				if(assembly != null) 
				{ 
					//Get all classes in this assembly
					Type[] types = assembly.GetTypes();
					//For each type in the array
					foreach(Type type in types) 
					{ 
						//Skip if it is an interface or abstract class
						if(type.IsInterface || type.IsAbstract) 
						{ 
							continue; 
						} 
						else 
						{ 
							//Add it to AvaliableKIs if the type derives from BattleShipKI
							if(typeof(BattleshipKI).IsAssignableFrom (type)) 
							{ 
								AvaliableKIs.Add (type);
							} 
						} 
					} 
				} 
			}
		}

		public static BattleshipKI NewKi(Type type, int size){
			//Searching for the main Constructor
			ConstructorInfo ctor = type.GetConstructor(new[] { typeof(int) });
			//Create a new instance
			object instance = ctor.Invoke(new object[] { size });
			return (BattleshipKI)instance;
		}
	}
}

