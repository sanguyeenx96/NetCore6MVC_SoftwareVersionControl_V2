$(".btnEdit").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    var name = $(this).data("name");
    var idStr = $(this).data("id");
    var id = parseInt(idStr);

    $.ajax({
        url: "/Home/GetInfoIdSoft",
        type: "GET",
        data: { softId: id },
        success: function (result) {
            $("#modelContent_Suathongtinphanmem").html(result);
            $("#modalSuathongtinphanmem").modal("show");
        },
        error: function (xhr, status, error) {
            Swal.fire(
                'Không tải được dữ liệu Model',
                error,
                'error'
            )
        }
    });


});
