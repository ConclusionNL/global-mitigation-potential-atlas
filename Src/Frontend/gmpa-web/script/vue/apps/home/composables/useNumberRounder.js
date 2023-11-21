export function useNumberRounder() {
    function sizeBasedRound(number) {
      if (number < 1) {
        return Math.round(number * 1000) / 1000; // Round to three decimal places if less than 1
      } else if (number < 10) {
        return Math.round(number * 100) / 100; // Round to two decimal places if less than 10
      } else if (number < 100){
        return Math.round(number * 10) / 10; // Round to one decimal place if less than 100
      } else if (number < 1000) {
        return Math.round(number); // Round to nearest integer if less than 1000
      } else {
        return Math.round(number / 1000) * 1000; // Round to nearest thousand if greater than or equal to 1000
      }
    }
  
    return {
      sizeBasedRound,
    };
  }