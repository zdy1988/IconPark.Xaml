const fs = require("fs");
var os = require("os");

const fsOpt = { encoding: "utf8" };

const unrealizedFile = `${__dirname}/unrealized.md`;

function writeHeader() {
  if (fs.existsSync(unrealizedFile)) {
    fs.unlinkSync(unrealizedFile);
  }

  fs.appendFileSync(unrealizedFile, "| Name      | SvgPath |" + os.EOL, fsOpt);
  fs.appendFileSync(unrealizedFile, "| ----------- | ----------- |" + os.EOL, fsOpt);
}

function writeItem(name, svgPath) {
  fs.appendFileSync(unrealizedFile, `| ${name}      | ${svgPath}       |` + os.EOL, fsOpt);
}

var UnrealizedFile = {
  writeHeader: writeHeader,
  writeItem: writeItem,
};

module.exports = UnrealizedFile;
