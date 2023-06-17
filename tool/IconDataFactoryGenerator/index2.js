const fs = require("fs");
var os = require("os");

const { getSvgPath, convertToHumpName } = require("./tools.js");

const IconKindFile = require("./icon-kind-file.js");

const data = require("./icons.json");

IconKindFile.writeHeader();

for (var i = 0; i < data.length; i++) {
  var item = data[i];
  var title = item.title;
  var name = convertToHumpName(`-${item.name}`);
  var category = `${item.category}|${item.categoryCN}`;

  IconKindFile.writeItem(name, title, category);

  console.log(`${item.id}.${item.name}`);
}

IconKindFile.writeFooter();

console.log("write over！");
