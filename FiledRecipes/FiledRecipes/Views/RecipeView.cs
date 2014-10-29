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

            Header = recipe.Name;
            ShowHeaderPanel();

            ShowPanel("Ingredienser", App.Controls.MessagePanelOptions.Basic);
            foreach(Ingredient ingred in recipe.Ingredients){

                Console.WriteLine(ingred);
            }

            ShowPanel("Instruktioner", App.Controls.MessagePanelOptions.Basic);
            foreach (string instr in recipe.Instructions)
            {
                Console.WriteLine(instr);
            }

        }
        public virtual void Show(IEnumerable<IRecipe> recipes)
        {

            foreach (IRecipe recipe in recipes)
            {
                Header = recipe.Name;
                ShowHeaderPanel();

                ShowPanel("Ingredienser", App.Controls.MessagePanelOptions.Basic);
                foreach (Ingredient ingred in recipe.Ingredients)
                {

                    Console.WriteLine(ingred);
                }
              
                ShowPanel("Instruktioner", App.Controls.MessagePanelOptions.Basic);
                foreach (string instr in recipe.Instructions)
                {
                    Console.WriteLine(instr);
                }

            }

            ContinueOnKeyPressed();


        }

    }
}
