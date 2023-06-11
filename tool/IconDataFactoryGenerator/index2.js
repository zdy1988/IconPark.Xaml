const fs = require("fs");
var os = require("os");

const { findSvgFileSync, makeSvgPath, convertToHumpName } = require("./tools.js");

const IconDataFactoryFile = require("./icon-data-factory.js");
const IconKindFile = require("./icon-kind.js");

const svgDir = `.//source`;

IconDataFactoryFile.writeHeader();
IconKindFile.writeHeader();

let i = 1;

findSvgFileSync(svgDir, function (filePath, filename, dirent) {
  console.log(`${i}.${filename}`);
  var svgPath = makeSvgPath(filePath);
  var names = filename.split("_");
  var title = names[0];
  var name = convertToHumpName(`-${names[1]}`);

  IconDataFactoryFile.writeItem(name, svgPath);
  IconKindFile.writeItem(name, title);

  i++;
});

IconDataFactoryFile.writeFooter();
IconKindFile.writeFooter();

console.log("write over！");
