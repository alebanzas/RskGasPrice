using System;
using System.Collections.Generic;
using GasPrice.Data.Models;

namespace GasPrice.Data.Interfaces
{
    public interface IGasPersistor
    {
        List<GasMeasurement> Get(DateTime? date);

        void Save(GasMeasurement gas);
    }
}
