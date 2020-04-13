function getInnerCodeCellText(cell) {
    return cell.innerText;
}
var editors;
function createCodeCell(cell) {

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