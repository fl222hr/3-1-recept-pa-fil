using FiledRecipes.Domain;
using FiledRecipes.App.Mvp;
using FiledRecipes.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiledRecipes.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class RecipeView : ViewBase, IRecipeView
    {

        public virtual void Show(IRecipe recipe)
        {
            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine(recipe.Name);
            Console.WriteLine("");
            Console.WriteLine("-------------------Ingredienser---------------------");
            foreach(Ingredient ingred in recipe.Ingredients){

                Console.WriteLine(ingred);
            }
            Console.WriteLine("");
            Console.WriteLine("-------------------Instruktioner---------------------");
            foreach (string instr in recipe.Instructions)
            {
                Console.WriteLine(instr);
            }

        }
        public virtual void Show(IEnumerable<IRecipe> recipes)
        {

            foreach (IRecipe recipe in recipes)
            {
                Console.WriteLine("");
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine(recipe.Name);
                Console.WriteLine("");
                Console.WriteLine("-------------------Ingredienser---------------------");
                foreach (Ingredient ingred in recipe.Ingredients)
                {

                    Console.WriteLine(ingred);
                }
                Console.WriteLine("");
                Console.WriteLine("-------------------Instruktioner---------------------");
                foreach (string instr in recipe.Instructions)
                {
                    Console.WriteLine(instr);
                }


            }
            Console.WriteLine("");
            Console.WriteLine("Tryck på tangent för att fortsätta");
            Console.ReadKey();


        }

    }
}
