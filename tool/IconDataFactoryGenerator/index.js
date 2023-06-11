const fs = require("fs");
var os = require("os");

const { getSvgPath, convertToHumpName } = require("./tools.js");

const IconDataFactoryFile = require("./icon-data-factory.js");
const IconKindFile = require("./icon-kind.js");

const data = require('./icons.json')

IconDataFactoryFile.writeHeader();
IconKindFile.writeHeader();

for(var i=0;i<data.length;i++)
{
    var item  = data[i];
    var svgPath = getSvgPath(item.svg);
    var title = item.title;
    var name = convertToHumpName(`-${item.name}`);

    IconDataFactoryFile.writeItem(name, svgPath);
    IconKindFile.writeItem(name, title);

    console.log(`${item.id}.${item.name}`);
}

IconDataFactoryFile.writeFooter();
IconKindFile.writeFooter();

console.log("write over！");
