const fs = require("fs");
var os = require("os");

const fsOpt = { encoding: "utf8" };

const iconDataFactoryFile = `${__dirname}/IconDataFactory.cs`;

function writeHeader() {
  if (fs.existsSync(iconDataFactoryFile)) {
    fs.unlinkSync(iconDataFactoryFile);
  }

  fs.appendFileSync(iconDataFactoryFile, "using System;" + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, "using System.Collections.Generic;" + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, " " + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, "namespace IconPark.Xaml" + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, "{" + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, "    internal class IconDataFactory" + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, "    {" + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, "        internal static IDictionary<IconKind, string> Create() => new Dictionary<IconKind, string>" + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, "        {" + os.EOL, fsOpt);
}

function writeItem(name, svgPath) {
  fs.appendFileSync(iconDataFactoryFile, `            {IconKind.${name},"${svgPath}"},` + os.EOL, fsOpt);
}

function writeFooter() {
  fs.appendFileSync(iconDataFactoryFile, "        };" + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, "    }" + os.EOL, fsOpt);
  fs.appendFileSync(iconDataFactoryFile, "}" + os.EOL, fsOpt);
}

var IconDataFactoryFile = {
    writeHeader : writeHeader,
    writeItem : writeItem,
    writeFooter : writeFooter
}

module.exports = IconDataFactoryFile
