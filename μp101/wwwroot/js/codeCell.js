function getInnerCodeCellText(cell) {
    return cell.innerText;
}
function createCodeCell(cell) {
    var myCodeMirror = CodeMirror(document.body, {
        value: "function myScript(){return 100;}\n",
        mode: "asm86",
        theme: "yonce",
        lineNumbers: true
    });
    var editors = CodeMirror(document.getElementById(cell), {
        value: "function  j00H(){console.log('PK')}",
        theme:"yonce",
        mode:"asm86"
    });
}