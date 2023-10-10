$(".dropdown-item.changeNamePhanmem").on("click", function () {
    var softid = $(this).data("id");
    var modelid = $(this).data("modelid");

    $.ajax({
        url: "/Soft/checkToChangeNameSoft",
        type: "GET",
        data: { softId: softid },
        success: function (data) {
            if (data.success) {
                Swal.fire({
                    title: "Đổi tên phần mềm",
                    html: `
                        <input type="text" id="newName" class="swal2-input" placeholder="Nhập tên phần mềm mới" autocomplete="off" >
                        `,
                    confirmButtonText: "Xác nhận",
                    focusConfirm: false,
                    preConfirm: () => {
                        const tenMoi =
                            Swal.getPopup().querySelector("#newName").value;
                        if (!tenMoi) {
                            Swal.showValidationMessage(
                                `Phải nhập đầy đủ thông tin`
                            );
                        }
                        return { tenMoi: tenMoi };
                    },
                }).then((result) => {
                    var tenMoi = result.value.tenMoi.trim();
                    $.ajax({
                        url: "/Soft/changeNameSoft",
                        type: "POST",
                        data: { softId: softid, tenMoi: tenMoi },
                        success: function () {
                            Swal.fire(
                                "Thành công",
                                "Đã đổi tên phần mềm thành công!",
                                "success"
                            ).then(function () {
                                $.ajax({
                                    url: "/Soft/GetListSoftware",
                                    type: "GET",
                                    data: { modelId: modelid },
                                    success: function (data) {
                                        $("#modelInfo_Danhsachphanmem").html(
                                            data
                                        );
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
            } else {
                Swal.fire(
                    "Phần mềm đã có dữ liệu",
                    "Chỉ có thể đổi tên phần mềm chưa có dữ liệu!",
                    "error"
                );
            }
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
