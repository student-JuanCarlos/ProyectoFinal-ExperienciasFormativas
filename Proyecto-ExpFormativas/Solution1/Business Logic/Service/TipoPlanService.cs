using Data.DTOs.WebhookDTO;
using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class TipoPlanService : ITipoPlan
    {
        private readonly ITipoPlan planDB;

        public TipoPlanService(ITipoPlan service)
        {
            planDB = service;
        }

        public TipoPlan Detalle(int id)
        {
            return planDB.Detalle(id);
        }

        public List<TipoPlan> ListarPlanesActivos()
        {
            return planDB.ListarPlanesActivos();
        }

    }
}
