<div class="page-breadcrumb">
    <div class="row">
        <div class="col-5 align-self-center">
            <h4 class="page-title">Tentang</h4>
            <div class="d-flex align-items-center">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item"><a href="javascript:void(0)">Administrasi</a></li>
                        <li class="breadcrumb-item"><a href="javascript:void(0)">Tentang</a></li>
                    </ol>
                </nav>
            </div>
        </div>
        <div class="col-7 align-self-center">
            <div class="d-flex no-block justify-content-end align-items-center">
                <button onclick="routing('tentang')" type="button" class="btn waves-effect waves-light btn-rounded btn-primary"><i class="fas fa-redo"></i> Refresh Page</button>
            </div>
        </div>
    </div>
</div>
<div class="container-fluid">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-body" id="load-data">
                    <div class="row">
                        <div class="col-5 align-self-center">
                            <h4 class="page-title">Master Data Tentang</h4>
                        </div>
                    </div><br>
                    <form class="m-t-30" id="data-update" onsubmit="update_data(event)">
                        <div class="form-group">
                            <label>Tentang</label>
                            <textarea name="deskripsi" id="tentang" rows="8" style="width: 100%;"></textarea>
                        </div>
                        <input type="hidden" id="id_tentang" name="id" value="{{id}}">
                        <button type="submit" class="btn btn-primary">Ubah</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

<script>
    var quill;
    var accesstoken = <?php echo json_encode($_COOKIE['accesstoken']); ?>; 
    $(document).ready(function() { 
        $.ajax({
            url: url_api+"Tentang(1)",
            type: 'GET',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer '+accesstoken+'');
            },
            dataType: 'json',
            success: function (data, textStatus, xhr) {
                // edit(data)

                $('#id_tentang').val(data.id);
                $('#tentang').val(data.deskripsi);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    });
    function update_data(e) {
        e.preventDefault();
        var data = $('#data-update').serializeFormJSON();
        // var data_quill = quill.getContents();
        // data.deskripsi = JSON.stringify(data_quill)
        // console.log(JSON.stringify(data))
        $.ajax({
            url: url_api+'Tentang(1)',
            type: 'PATCH',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer '+accesstoken+'');
            },
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (data, textStatus, xhr) {
                if (xhr.status == '204') {
                    routing('tentang');
                    toastr.success("Memperbarui Tentang", 'Berhasil!');
                }else{
                    toastr.error("Memperbarui Tentang", 'Gagal!');
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    }
</script>
