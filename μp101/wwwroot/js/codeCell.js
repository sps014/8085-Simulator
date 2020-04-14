function getInnerCodeCellText(cell) {
    return cell.innerText;
}
var editors = new Object();
function createCodeCell(cell) {

    try {
        editors[cell] = CodeMirror.fromTextArea(document.getElementById(cell), {
            value: "function  j00H(){console.log('PK')}",
            theme: "yonce",
            mode: "asm86",
            lineNumbers: true,
            viewportMargin: Infinity
        });
    }
    catch (e) {
        console.error(e);
    }
}