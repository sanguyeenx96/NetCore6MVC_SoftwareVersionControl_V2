$(".btnXemchitiet").click(function () {
    var id = $(this).data("id");
    var name = $(this).data("name");

    $.ajax({
        type: "GET",
        url: "/Soft/GetListAllVersionOfSoftware",
        data: { softId: id },
        success: function (response) {
            console.log(response);
            // Lấy dữ liệu từ response.data
            var data = response.data;
            // Tạo biến HTML cho bảng dữ liệu
            var tableHtml =
                '<table id="tableListAllVersionOfSoftware" class="table table-striped" style="font-size:small">';
            tableHtml +=
                '<thead><tr> <th>Version</th> <th>Trạng thái áp dụng</th> <th>Điểm thay đổi</th> <th class="none">Ngày Áp Dụng</th> <th class="none">Ngày Cài Đặt</th> <th class="none">Số Lượng Jig Cài Đặt</th> <th class="none">Soft Name</th> <th class="none">File Name</th> <th class="none">Path</th> <th class="none">Người Cài Đặt</th> <th class="none">Người Xác Nhận</th> <th class="none">Upload At</th> </tr></thead>';
            tableHtml += "<tbody>";

            // Lặp qua dữ liệu và thêm từng dòng vào bảng
            $.each(data, function (index, item) {
                tableHtml += "<tr>";
                tableHtml += "<td class='col-2'>" + item.version + "</td>";
                tableHtml +=
                    "<td  class='col-3'>" +
                    (item.trangThaiApDung
                        ? '<span class="badge badge-success">Đang áp dụng</span>'
                        : '<span class="badge badge-danger">Không áp dụng</span>') +
                    "</td>";
                tableHtml += "<td  class='col-7'>" + item.diemThayDoi + "</td>";

                tableHtml += "<td>" + item.ngayApDung + "</td>";
                tableHtml += "<td>" + item.ngayCaiDat + "</td>";
                tableHtml += "<td>" + item.soLuongJigCaiDat + "</td>";
                tableHtml += "<td>" + item.softName + "</td>";
                tableHtml += "<td>" + item.fileName + "</td>";
                tableHtml += "<td>" + item.path + "</td>";
                tableHtml += "<td>" + item.nguoiCaiDat + "</td>";
                tableHtml += "<td>" + item.nguoiXacNhan + "</td>";
                tableHtml += "<td>" + item.timeCreated + "</td>";
                tableHtml += "</tr>";
            });

            tableHtml += "</tbody></table>";

            Swal.fire({
                icon: 'info',
                title: name,
                html: tableHtml, // Sử dụng biến HTML chứa bảng
                position: "top-start",
                showClass: {
                    popup: `
                        animate__animated
                        animate__fadeInLeft
                        animate__faster
                    `,
                },
                hideClass: {
                    popup: `
                        animate__animated
                        animate__fadeOutLeft
                        animate__faster
                    `,
                },
                grow: "column",
                width: 800,
                showConfirmButton: false,
                showCloseButton: true,
            });
            if (Swal.isVisible()) {
                // Tạo DataTable sau khi Swal đã hiển thị
                $("#tableListAllVersionOfSoftware")
                    .DataTable({
                        language: {
                            processing: "Đang tải dữ liệu...",
                            search: "Tìm kiếm",
                            infoEmpty: '<span class="badge bg-gradient-danger">Không có dữ liệu</span>',
                            loadingRecords: "",
                            zeroRecords: "Không tìm thấy bản ghi phù hợp",
                            emptyTable: "Không có dữ liệu",
                            paginate: {
                                first: "Trang đầu",
                                previous: "Trang trước",
                                next: "Trang sau",
                                last: "Trang cuối",
                            },
                            aria: {
                                sortAscending: ": Đang sắp xếp theo column",
                                sortDescending: ": Đang sắp xếp theo column",
                            },
                        },
                        
                        responsive: true,
                        lengthChange: true,
                        order: [[1, 'desc']], // Sắp xếp theo cột thứ hai (index 1) theo thứ tự tăng dần (asc)
                        buttons: ["copy", "excel", "print"],
                    })
                    .buttons()
                    .container()
                    .appendTo("#example1_wrapper .col-md-6:eq(0)");
            }
        },
        error: function () {
            console.log("Đã xảy ra lỗi khi gọi API.");
        },
    });
});
