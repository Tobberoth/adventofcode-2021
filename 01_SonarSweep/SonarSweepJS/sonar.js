const fs = require('fs')
const input = ReadInput("input.txt")
console.log(GetIncreases(input))
const sliding = GetSliding(input, 3)
console.log(GetIncreases(sliding))

function ReadInput(filename)
{
  return fs.readFileSync(filename, "utf8").split("\n").map(s => parseInt(s));
}

function GetIncreases(array)
{
  return array.reduce((increases, num, index, ary) => {
    if (num > ary[index-1]) increases++;
    return increases;
  },0)
}

function GetSliding(inputArray, windowSize)
{
  return inputArray.reduce((filtered, _, index, array) => {
    if (index < array.length - windowSize + 1)
      filtered.push(array.slice(index, index + windowSize).reduce((acc, e) => acc + e, 0))
    return filtered;
  },[]);
}