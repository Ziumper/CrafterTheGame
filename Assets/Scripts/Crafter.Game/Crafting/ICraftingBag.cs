using Crafter.Game.Equipment;

namespace Crafter.Game.Crafting
{
    public interface ICraftingBag
    {
        void CraftFrom(CraftingRecipe recipe);
    }
}
