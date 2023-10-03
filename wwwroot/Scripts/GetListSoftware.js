$(".xemDanhSachPhanMem").on("click", function () {
    var modelName = $(this).data("model-name");
    var itemId = $(this).closest("tr").data("item-id");
    $.ajax({
        url: "/Soft/GetListSoftware",
        type: "GET",
        data: { modelId: itemId },
        success: function (data) {
            $("#modelInfo_Danhsachphanmem").html(data);
            $("#modalDanhsachphanmem")
                .find(".modal-title-content")
                .text("Danh sách phần mềm Model " + modelName);
            $("#modalDanhsachphanmem").modal("show");
        },
        error: function (xhr, t, error) {
            console.log(error);
            Swal.fire(
                "Không lấy được dữ liệu",
                "Thao tác đã được huỷ bỏ!",
                "error"
            );
        },
    });
});
