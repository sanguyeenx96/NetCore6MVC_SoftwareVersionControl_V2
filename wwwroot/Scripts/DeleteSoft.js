$(".dropdown-item.deletePhanmem").on("click", function () {
    var id = $(this).data("id");
    var modelid = $(this).data("modelid");

    const swalWithBootstrapButtons = Swal.mixin({
        buttonsStyling: true,
    });
    swalWithBootstrapButtons
        .fire({
            title: "Chọn chế độ xoá dữ liệu",
            icon: "question",
            showDenyButton: true,
            confirmButtonText: "Xoá tất cả Version",
            denyButtonText: "Chọn Version để xoá",
            reverseButtons: false,
        })
        .then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: "xoá tất cả Version?",
                    text: "Dữ liệu sẽ bị xoá vĩnh viễn",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Xác nhận xoá",
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            url: "/Soft/DeleteAllVerOfSoft",
                            type: "POST",
                            data: { softid: id },
                            success: function (data) {
                                if (!data.success) {
                                    Swal.fire({
                                        icon: "error",
                                        title: "Oops...",
                                        text: "Có lỗi xảy ra",
                                    });
                                } else {
                                    Swal.fire(
                                        "Thành công",
                                        "Bạn vừa xoá phần mềm thành công",
                                        "success"
                                    ).then(function () {
                                        $.ajax({
                                            url: "/Soft/GetListSoftware",
                                            type: "GET",
                                            data: { modelId: modelid },
                                            success: function (data) {
                                                $(
                                                    "#modelInfo_Danhsachphanmem"
                                                ).html(data);
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
                                }
                            },
                            error: function (xhr, status, error) {
                                Swal.fire({
                                    icon: "error",
                                    title: "Oops...",
                                    text: error,
                                });
                            },
                        });
                    }
                });
            } else if (result.isDenied) {
                $.ajax({
                    url: "/Soft/GetSelectListVersion",
                    method: "GET",
                    data: { id: id },
                    success: function (data) {
                        console.log(data);
                        const vers = data;
                        const inputOptions = {};
                        vers.forEach((ver) => {
                            inputOptions[ver.id] = ver.version;
                        });
                        swalWithBootstrapButtons
                            .fire({
                                title: "Chọn Version muốn xoá",
                                input: "select",
                                inputOptions: inputOptions,
                                showCancelButton: true,
                                confirmButtonText: "Xoá Version",
                                cancelButtonText: "Huỷ bỏ",
                                reverseButtons: true,
                            })
                            .then((result) => {
                                if (result.isConfirmed) {
                                    var ver = result.value;
                                    $.ajax({
                                        url: "/Soft/Delete1VerOfSoft",
                                        type: "POST",
                                        data: { softid: id, verSoftId: ver },
                                        success: function (data) {
                                            Swal.fire(
                                                "Thành công",
                                                "Bạn vừa xoá phần mềm thành công",
                                                "success"
                                            ).then(function () {
                                                $.ajax({
                                                    url: "/Soft/GetListSoftware",
                                                    type: "GET",
                                                    data: { modelId: modelid },
                                                    success: function (data) {
                                                        $(
                                                            "#modelInfo_Danhsachphanmem"
                                                        ).html(data);
                                                    },
                                                    error: function (
                                                        xhr,
                                                        t,
                                                        error
                                                    ) {
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
                                } else if (
                                    result.dismiss === Swal.DismissReason.cancel
                                ) {
                                    swalWithBootstrapButtons.fire(
                                        "Đã huỷ",
                                        "Thao tác đã được huỷ bỏ!",
                                        "error"
                                    );
                                }
                            });
                    },
                    error: function () {
                        Swal.fire(
                            "Lỗi máy chủ",
                            "Không tải được dữ liệu",
                            "error"
                        );
                    },
                });
            }
        });
});
