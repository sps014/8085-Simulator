function getInnerCodeCellText(cell) {
    return cell.innerText;
}
var editors;
function createCodeCell(cell) {
    var myCodeMirror = CodeMirror(document.body, {
        value: "function myScript(){return 100;}\n",
        mode: "asm86",
        theme: "yonce",
        lineNumbers: true
    });
    try {
        editors = CodeMirror.fromTextArea(document.getElementById(cell), {
            value: "function  j00H(){console.log('PK')}",
            theme: "yonce",
            mode: "asm86",
            lineNumbers: true
        });
    }
    catch (e) {
        createCodeCell(cell);
    }
}