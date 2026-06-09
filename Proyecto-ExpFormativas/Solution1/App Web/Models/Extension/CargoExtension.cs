using App_Web.Models.VM;
using Entities;

namespace App_Web.Models.Extension
{
    public static class CargoExtension
    {

        public static CargoVM ToViewModel(this Cargo cargo)
        {
            return new CargoVM()
            {
                IdCargo = cargo.IdCargo,
                NombreCargo = cargo.NombreCargo,
                Descripcion = cargo.Descripcion == null ? null : cargo.Descripcion
            };
        }

        public static Cargo ToEntity(this CargoVM model)
        {
            return new Cargo()
            {
                IdCargo = model.IdCargo,
                NombreCargo = model.NombreCargo,
                Descripcion = model.Descripcion == null ? null : model.Descripcion
            };
        }

    }
}
