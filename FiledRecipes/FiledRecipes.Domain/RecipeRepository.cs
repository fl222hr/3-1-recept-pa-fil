using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiledRecipes.Domain
{
    /// <summary>
    /// Holder for recipes.
    /// </summary>
    public class RecipeRepository : IRecipeRepository
    {
        /// <summary>
        /// Represents the recipe section.
        /// </summary>
        private const string SectionRecipe = "[Recept]";

        /// <summary>
        /// Represents the ingredients section.
        /// </summary>
        private const string SectionIngredients = "[Ingredienser]";

        /// <summary>
        /// Represents the instructions section.
        /// </summary>
        private const string SectionInstructions = "[Instruktioner]";

        /// <summary>
        /// Occurs after changes to the underlying collection of recipes.
        /// </summary>
        public event EventHandler RecipesChangedEvent;

        /// <summary>
        /// Specifies how the next line read from the file will be interpreted.
        /// </summary>
        private enum RecipeReadStatus { Indefinite, New, Ingredient, Instruction };

        /// <summary>
        /// Collection of recipes.
        /// </summary>
        private List<IRecipe> _recipes;

        /// <summary>
        /// The fully qualified path and name of the file with recipes.
        /// </summary>
        private string _path;

        /// <summary>
        /// Indicates whether the collection of recipes has been modified since it was last saved.
        /// </summary>
        public bool IsModified { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the RecipeRepository class.
        /// </summary>
        /// <param name="path">The path and name of the file with recipes.</param>
        public RecipeRepository(string path)
        {
            // Throws an exception if the path is invalid.
            _path = Path.GetFullPath(path);

            _recipes = new List<IRecipe>();
        }

        /// <summary>
        /// Returns a collection of recipes.
        /// </summary>
        /// <returns>A IEnumerable&lt;Recipe&gt; containing all the recipes.</returns>
        public virtual IEnumerable<IRecipe> GetAll()
        {
            // Deep copy the objects to avoid privacy leaks.
            return _recipes.Select(r => (IRecipe)r.Clone());
        }

        /// <summary>
        /// Returns a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to get.</param>
        /// <returns>The recipe at the specified index.</returns>
        public virtual IRecipe GetAt(int index)
        {
            // Deep copy the object to avoid privacy leak.
            return (IRecipe)_recipes[index].Clone();
        }

        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="recipe">The recipe to delete. The value can be null.</param>
        public virtual void Delete(IRecipe recipe)
        {
            // If it's a copy of a recipe...
            if (!_recipes.Contains(recipe))
            {
                // ...try to find the original!
                recipe = _recipes.Find(r => r.Equals(recipe));
            }
            _recipes.Remove(recipe);
            IsModified = true;
            OnRecipesChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to delete.</param>
        public virtual void Delete(int index)
        {
            Delete(_recipes[index]);
        }

        /// <summary>
        /// Raises the RecipesChanged event.
        /// </summary>
        /// <param name="e">The EventArgs that contains the event data.</param>
        protected virtual void OnRecipesChanged(EventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler handler = RecipesChangedEvent;

            // Event will be null if there are no subscribers. 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

        public virtual void Load()
        {
            Recipe recipe = new Recipe("Start");
            bool skip = false;
            string type = "";
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(_path);
            while ((line = file.ReadLine()) != null && line != "")
            {
                if (line == "[Recept]")
                {
                    skip = true;
                    type = "recipe";
                }

                if (line == "[Ingredienser]")
                {
                    skip = true;
                    type = "ingred";
                }

                if (line == "[Instruktioner]")
                {
                    skip = true;
                    type = "instr";
                }

                if (skip == false)
                {

                    switch (type)
                    {
                        case "recipe":
                            if (recipe.Name != "Start")
                            {
                                _recipes.Add(recipe);
                                Console.WriteLine("Added recipe ");
                            }
                            recipe = new Recipe(line);
                            Console.WriteLine("recipe name " + line);
                            break;
                        case "ingred":

                            string[] part = line.Split(';');
                            Ingredient ingredient = new Ingredient();
                            ingredient.Amount = part[0];
                            ingredient.Measure = part[1];
                            ingredient.Name = part[2];
                            recipe.Add(ingredient);
                            Console.WriteLine("Added ingred " + part[0] + part[1] + part[2]);
                            break;

                        case "instr":
                            recipe.Add(line);
                            Console.WriteLine("Added isntruction " +line);
                            break;

                        default:                   
                            break;
                    }

                }

                skip = false;
                

            }

            file.Close();
            _recipes.Add(recipe);
            Console.WriteLine("Added recipe");
            _recipes.Sort();
            OnRecipesChanged(EventArgs.Empty);

        }

        public virtual void Save()
        {

        }
    }
}
