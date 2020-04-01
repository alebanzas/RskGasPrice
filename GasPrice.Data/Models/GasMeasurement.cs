using System;

namespace GasPrice.Data.Models
{
    public class GasMeasurement
    {
        public GasMeasurement()
        {
            Moment = DateTime.UtcNow;
        }

        public DateTime Moment { get; set; }

        public decimal RSKMinGasPrice { get; set; }

        public decimal EthGasPriceLow { get; set; }

        public decimal EthGasPriceStandard { get; set; }

        public decimal EthGasPriceFast { get; set; }

        public decimal EthVsUsd { get; set; }

        public decimal BtcVsUsd { get; set; }


        public decimal RskGasPriceInUsd()
        {
            var rskInBtc = RSKMinGasPrice / 1000000000;

            return rskInBtc * BtcVsUsd * 21000;
        }

        public decimal EthGasPriceInUsd()
        {
            var ethInBtc = EthGasPriceStandard * 10 / 1000000000;

            return ethInBtc * EthVsUsd * 21000;
        }

        public bool IsValid()
        {
            return EthGasPriceStandard != 0 &&
                   RSKMinGasPrice != 0 &&
                   BtcVsUsd != 0 &&
                   EthVsUsd != 0;

        }
    }
}
