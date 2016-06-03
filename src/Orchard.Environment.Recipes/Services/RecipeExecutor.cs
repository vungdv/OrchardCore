﻿using Orchard.Environment.Recipes.Models;
using Orchard.Environment.Shell.Descriptor;

namespace Orchard.Environment.Recipes.Services
{
    public class RecipeExecutor : IRecipeExecutor
    {
        private readonly IRecipeManager _recipeManager;
        private readonly IShellDescriptorManager _shellDescriptorManager;

        public RecipeExecutor(
            IRecipeParser recipeParser,
            IRecipeManager recipeManager,
            IShellDescriptorManager shellDescriptorManager)
        {
            _recipeManager = recipeManager;
            _shellDescriptorManager = shellDescriptorManager;
        }

        public string Execute(Recipe recipe)
        {
            var executionId = _recipeManager.Execute(recipe);

            // Only need to update the shell if work was actually done.
            if (executionId != null)
                UpdateShell();

            return executionId;
        }

        private void UpdateShell()
        {
            var descriptor = _shellDescriptorManager.GetShellDescriptorAsync().Result;
            _shellDescriptorManager.UpdateShellDescriptorAsync(descriptor.SerialNumber, descriptor.Features, descriptor.Parameters).Wait();
        }
    }
}