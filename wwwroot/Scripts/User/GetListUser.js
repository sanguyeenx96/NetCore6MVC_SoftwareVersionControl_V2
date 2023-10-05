$("#btnQuanlytaikhoan").click(function (e) {
    $.ajax({
        url: "/User/GetDanhsachtaikhoan",
        type: "GET",
        success: function (data) {
            $("#modal-quanlytaikhoan").modal("show");
            $("#modalContent_Quanlytaikhoan").html(
                data
            );
        },
        error: function (xhr, t, error) {
            Swal.fire(
                "Không lấy được dữ liệu",
                "Thao tác đã được huỷ bỏ!",
                "error"
            );
        },
    });
});
