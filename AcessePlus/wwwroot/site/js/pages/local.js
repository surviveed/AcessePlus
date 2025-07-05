function abrirModal(nomeLocal, idLocal) {
    $("#nomeLocalSpan").text(nomeLocal);
    $("#avaliarModal").show();
    $("#comentario").val('');
    $("#LocalId").val(idLocal);
    $("#tipoAcessibilidade").prop('selectedIndex', 0);
    $("#tipo").prop('selectedIndex', 0);
}


function fecharModal() {
    document.getElementById("avaliarModal").style.display = "none";
}

window.onclick = function (event) {
    const modal = document.getElementById("avaliarModal");
    if (event.target === modal) {
        modal.style.display = "none";
    }
};

function atualizarContadorComentario() {
    const textarea = document.getElementById("comentario");
    const contador = document.getElementById("contadorComentario");
    contador.textContent = `${textarea.value.length} / 2000`;
}

document.addEventListener("DOMContentLoaded", function () {
    const textarea = document.getElementById("comentario");
    if (textarea) {
        textarea.addEventListener("input", atualizarContadorComentario);
    }
});