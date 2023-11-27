export function useNumberRounder() {
    function sizeBasedRound(number) {
      let decimalPlaces = 0;

    if (number < 1) {
      decimalPlaces = 3; // Round to three decimal places if less than 1
    } else if (number < 10) {
      decimalPlaces = 2; // Round to two decimal places if less than 10
    } else if (number < 100) {
      decimalPlaces = 1; // Round to one decimal place if less than 100
    }

    const power = Math.pow(10, decimalPlaces);
    const roundedNumber = Math.round(number * power) / power;

    return roundedNumber;
  }
  
    return {
      sizeBasedRound,
    };
  }