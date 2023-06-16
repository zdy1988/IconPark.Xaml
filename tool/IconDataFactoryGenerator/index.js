const fs = require("fs");
var os = require("os");

const { getSvgPath, convertToHumpName } = require("./tools.js");

const IconDataFactoryFile = require("./icon-data-factory-file.js");
const IconKindFile = require("./icon-kind-file.js");
const UnrealizedFile = require("./unrealized-file.js");

const data = require("./icons.json");

IconDataFactoryFile.writeHeader();
IconKindFile.writeHeader();
UnrealizedFile.writeHeader();

for (var i = 0; i < data.length; i++) {
  var item = data[i];
  var svgPath = getSvgPath(item.svg);
  var title = item.title;
  var name = convertToHumpName(`-${item.name}`);
  var category = `${item.category}|${item.categoryCN}`;

  if (svgPath == "" || svgPath.indexOf("null") != -1) {
    UnrealizedFile.writeItem(name, svgPath);
  } else {
    IconDataFactoryFile.writeItem(name, svgPath);
    IconKindFile.writeItem(name, title, category);
  }

  console.log(`${item.id}.${item.name}`);
}

IconDataFactoryFile.writeFooter();
IconKindFile.writeFooter();

console.log("write over！");
