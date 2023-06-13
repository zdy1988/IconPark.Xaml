const fs = require("fs");
var os = require("os");

const fsOpt = { encoding: "utf8" };

const iconKindFile = `${__dirname}/IconKind.cs`;

function writeHeader() {
  if (fs.existsSync(iconKindFile)) {
    fs.unlinkSync(iconKindFile);
  }

  fs.appendFileSync(iconKindFile, "using System.ComponentModel;" + os.EOL, fsOpt);
  fs.appendFileSync(iconKindFile, " " + os.EOL, fsOpt);
  fs.appendFileSync(iconKindFile, "namespace IconPark.Xaml" + os.EOL, fsOpt);
  fs.appendFileSync(iconKindFile, "{" + os.EOL, fsOpt);
  fs.appendFileSync(iconKindFile, "    public enum IconKind" + os.EOL, fsOpt);
  fs.appendFileSync(iconKindFile, "    {" + os.EOL, fsOpt);
}

function writeItem(name, title) {
  fs.appendFileSync(iconKindFile, `        [Description("${title}")] ${name},` + os.EOL, fsOpt);
}

function writeFooter() {
  fs.appendFileSync(iconKindFile, "    }" + os.EOL, fsOpt);
  fs.appendFileSync(iconKindFile, "}" + os.EOL, fsOpt);
}

var IconKindFile = {
  writeHeader: writeHeader,
  writeItem: writeItem,
  writeFooter: writeFooter,
};

module.exports = IconKindFile;
