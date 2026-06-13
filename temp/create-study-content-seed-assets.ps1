$ErrorActionPreference = 'Stop'

$root = Join-Path $PWD 'QuanLyTrungTam/backend/wwwroot/uploads/study-materials/seed/teacher-study-content'
$tempRoot = Join-Path $PWD 'temp/study-content-seed-assets'
New-Item -ItemType Directory -Force -Path $root | Out-Null
if (Test-Path $tempRoot) {
    Remove-Item -LiteralPath $tempRoot -Recurse -Force
}
New-Item -ItemType Directory -Force -Path $tempRoot | Out-Null

function Write-TextFileUtf8NoBom {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][string]$Content
    )

    $directory = Split-Path -Parent $Path
    New-Item -ItemType Directory -Force -Path $directory | Out-Null
    $utf8NoBom = New-Object System.Text.UTF8Encoding($false)
    [System.IO.File]::WriteAllText($Path, $Content, $utf8NoBom)
}

function New-MinimalPdf {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][string[]]$Lines
    )

    $escapedLines = foreach ($line in $Lines) {
        $line.Replace('\\', '\\\\').Replace('(', '\\(').Replace(')', '\\)')
    }

    $contentBuilder = New-Object System.Text.StringBuilder
    [void]$contentBuilder.Append("BT`n/F1 18 Tf`n72 760 Td`n")
    for ($i = 0; $i -lt $escapedLines.Count; $i++) {
        if ($i -eq 0) {
            [void]$contentBuilder.AppendFormat("({0}) Tj`n", $escapedLines[$i])
        } else {
            [void]$contentBuilder.AppendFormat("0 -24 Td ({0}) Tj`n", $escapedLines[$i])
        }
    }
    [void]$contentBuilder.Append("ET")
    $streamText = $contentBuilder.ToString()
    $streamLength = [System.Text.Encoding]::ASCII.GetByteCount($streamText)

    $objects = @(
        "1 0 obj`n<< /Type /Catalog /Pages 2 0 R >>`nendobj`n",
        "2 0 obj`n<< /Type /Pages /Count 1 /Kids [3 0 R] >>`nendobj`n",
        "3 0 obj`n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 5 0 R >> >> /Contents 4 0 R >>`nendobj`n",
        "4 0 obj`n<< /Length $streamLength >>`nstream`n$streamText`nendstream`nendobj`n",
        "5 0 obj`n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>`nendobj`n"
    )

    $header = "%PDF-1.4`n"
    $builder = New-Object System.Text.StringBuilder
    [void]$builder.Append($header)
    $offsets = New-Object System.Collections.Generic.List[int]

    foreach ($object in $objects) {
        $offsets.Add([System.Text.Encoding]::ASCII.GetByteCount($builder.ToString()))
        [void]$builder.Append($object)
    }

    $xrefOffset = [System.Text.Encoding]::ASCII.GetByteCount($builder.ToString())
    [void]$builder.Append("xref`n")
    [void]$builder.AppendFormat("0 {0}`n", $objects.Count + 1)
    [void]$builder.Append("0000000000 65535 f `n")
    foreach ($offset in $offsets) {
        [void]$builder.AppendFormat("{0:0000000000} 00000 n `n", $offset)
    }
    [void]$builder.Append("trailer`n")
    [void]$builder.AppendFormat("<< /Size {0} /Root 1 0 R >>`n", $objects.Count + 1)
    [void]$builder.Append("startxref`n")
    [void]$builder.AppendFormat("{0}`n", $xrefOffset)
    [void]$builder.Append("%%EOF`n")

    [System.IO.File]::WriteAllText($Path, $builder.ToString(), [System.Text.Encoding]::ASCII)
}

function New-ZipPackage {
    param(
        [Parameter(Mandatory = $true)][string]$SourceDirectory,
        [Parameter(Mandatory = $true)][string]$DestinationPath
    )

    if (Test-Path $DestinationPath) {
        Remove-Item -LiteralPath $DestinationPath -Force
    }

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::CreateFromDirectory($SourceDirectory, $DestinationPath)
}

function New-MinimalDocx {
    param([Parameter(Mandatory = $true)][string]$Path)

    $workDir = Join-Path $tempRoot 'docx'
    New-Item -ItemType Directory -Force -Path (Join-Path $workDir '_rels') | Out-Null
    New-Item -ItemType Directory -Force -Path (Join-Path $workDir 'word') | Out-Null

    Write-TextFileUtf8NoBom -Path (Join-Path $workDir '[Content_Types].xml') -Content @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
  <Default Extension="xml" ContentType="application/xml"/>
  <Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/>
</Types>
"@

    Write-TextFileUtf8NoBom -Path (Join-Path $workDir '_rels/.rels') -Content @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/>
</Relationships>
"@

    Write-TextFileUtf8NoBom -Path (Join-Path $workDir 'word/document.xml') -Content @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  <w:body>
    <w:p><w:r><w:t>Checklist giao tiep hang ngay</w:t></w:r></w:p>
    <w:p><w:r><w:t>1. Chao hoi va hoi tham nguoi doi dien.</w:t></w:r></w:p>
    <w:p><w:r><w:t>2. Gioi thieu ban than bang 2 den 3 cau ngan.</w:t></w:r></w:p>
    <w:p><w:r><w:t>3. Luyen mau cau hoi lai thong tin.</w:t></w:r></w:p>
    <w:p><w:r><w:t>4. Viet 5 cau hoi thoai co the dung trong lop.</w:t></w:r></w:p>
    <w:sectPr />
  </w:body>
</w:document>
"@

    New-ZipPackage -SourceDirectory $workDir -DestinationPath $Path
}

function New-MinimalXlsx {
    param([Parameter(Mandatory = $true)][string]$Path)

    $workDir = Join-Path $tempRoot 'xlsx'
    New-Item -ItemType Directory -Force -Path (Join-Path $workDir '_rels') | Out-Null
    New-Item -ItemType Directory -Force -Path (Join-Path $workDir 'xl/_rels') | Out-Null
    New-Item -ItemType Directory -Force -Path (Join-Path $workDir 'xl/worksheets') | Out-Null

    Write-TextFileUtf8NoBom -Path (Join-Path $workDir '[Content_Types].xml') -Content @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
  <Default Extension="xml" ContentType="application/xml"/>
  <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
  <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
  <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
</Types>
"@

    Write-TextFileUtf8NoBom -Path (Join-Path $workDir '_rels/.rels') -Content @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
</Relationships>
"@

    Write-TextFileUtf8NoBom -Path (Join-Path $workDir 'xl/workbook.xml') -Content @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
  <sheets>
    <sheet name="OnTap" sheetId="1" r:id="rId1"/>
  </sheets>
</workbook>
"@

    Write-TextFileUtf8NoBom -Path (Join-Path $workDir 'xl/_rels/workbook.xml.rels') -Content @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
  <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
</Relationships>
"@

    Write-TextFileUtf8NoBom -Path (Join-Path $workDir 'xl/styles.xml') -Content @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
  <fonts count="1"><font><sz val="11"/><name val="Calibri"/></font></fonts>
  <fills count="1"><fill><patternFill patternType="none"/></fill></fills>
  <borders count="1"><border/></borders>
  <cellStyleXfs count="1"><xf/></cellStyleXfs>
  <cellXfs count="1"><xf xfId="0"/></cellXfs>
</styleSheet>
"@

    Write-TextFileUtf8NoBom -Path (Join-Path $workDir 'xl/worksheets/sheet1.xml') -Content @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
  <sheetData>
    <row r="1">
      <c r="A1" t="inlineStr"><is><t>Chu de</t></is></c>
      <c r="B1" t="inlineStr"><is><t>Tu vung</t></is></c>
      <c r="C1" t="inlineStr"><is><t>Ghi chu</t></is></c>
    </row>
    <row r="2">
      <c r="A2" t="inlineStr"><is><t>Gia dinh</t></is></c>
      <c r="B2" t="inlineStr"><is><t>parents, siblings, relatives</t></is></c>
      <c r="C2" t="inlineStr"><is><t>On lai cach gioi thieu thanh vien.</t></is></c>
    </row>
    <row r="3">
      <c r="A3" t="inlineStr"><is><t>Cong viec</t></is></c>
      <c r="B3" t="inlineStr"><is><t>teacher, office, schedule</t></is></c>
      <c r="C3" t="inlineStr"><is><t>Luyen hoi dap ve nghe nghiep.</t></is></c>
    </row>
  </sheetData>
</worksheet>
"@

    New-ZipPackage -SourceDirectory $workDir -DestinationPath $Path
}

function New-MinimalWav {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [int]$SampleRate = 8000,
        [int]$DurationSeconds = 2
    )

    $samples = $SampleRate * $DurationSeconds
    $dataSize = $samples
    $memory = New-Object System.IO.MemoryStream
    $writer = New-Object System.IO.BinaryWriter($memory)

    $writer.Write([System.Text.Encoding]::ASCII.GetBytes('RIFF'))
    $writer.Write([int](36 + $dataSize))
    $writer.Write([System.Text.Encoding]::ASCII.GetBytes('WAVE'))
    $writer.Write([System.Text.Encoding]::ASCII.GetBytes('fmt '))
    $writer.Write([int]16)
    $writer.Write([short]1)
    $writer.Write([short]1)
    $writer.Write([int]$SampleRate)
    $writer.Write([int]$SampleRate)
    $writer.Write([short]1)
    $writer.Write([short]8)
    $writer.Write([System.Text.Encoding]::ASCII.GetBytes('data'))
    $writer.Write([int]$dataSize)

    for ($i = 0; $i -lt $samples; $i++) {
        $phase = [math]::Sin(2 * [math]::PI * 440 * $i / $SampleRate)
        $sample = [byte][math]::Round(128 + (24 * $phase))
        $writer.Write($sample)
    }

    $writer.Flush()
    [System.IO.File]::WriteAllBytes($Path, $memory.ToArray())
    $writer.Dispose()
    $memory.Dispose()
}

New-MinimalPdf -Path (Join-Path $root 'class-a-intro-guide.pdf') -Lines @(
    'English Education - Hoc chu dong demo',
    'Buoi 1: Chao hoi va gioi thieu ban than',
    '1. Tu vung chao hoi can nho.',
    '2. Mau cau gioi thieu ngan gon.',
    '3. Bai tap tu luyen sau buoi hoc.'
)

New-MinimalDocx -Path (Join-Path $root 'class-a-daily-conversation-checklist.docx')
New-MinimalXlsx -Path (Join-Path $root 'class-b-review-vocabulary.xlsx')
New-MinimalWav -Path (Join-Path $root 'toeic-part1-part2-listening-practice.wav')

Get-ChildItem -Path $root |
    Where-Object {
        $_.Name -match 'class-a-intro-guide|class-a-daily-conversation-checklist|class-b-review-vocabulary|toeic-part1-part2-listening-practice'
    } |
    Select-Object Name, Length, LastWriteTime