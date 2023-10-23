# Arlanet Frontend Library

## Including:
- Base project setup
- Arlanet Library for re-using components
- Storybook

## Nice to have/ ToDo:
- Configuration for Umbraco projects
- Configuration for epiServer projects
- Release notes

### How to install the Arlanet.Frontend.Library for the first time

Please follow the instructions in the file: [Frontend library installeren](stories/Installatie.mdx).

After installing this project you need to configure some files.

### How to update the Arlanet.Frontend.Library

Please follow the instructions in the file: [Frontend library update](stories/NieuweVersie.mdx).

## Configure [PROJECT-NAME].Frontend

### node / npm
- Use node version specified in the `.nvmrc` or `package.json` file.
  > Tip: Use [nvm-windows](https://github.com/coreybutler/nvm-windows) to easily switch between node versions.

### .vscode
- Move the `.vscode` directory from the `[PROJECT-NAME].Frontend` and paste it one level up within the root, next to the `X.Frontend` and `X.Web` directories. The new root directory structure should look like the following:
    ``` 
    |-- .vs
    |-- .vscode
    |-- [PROJECT-NAME].Frontend
    |-- [PROJECT-NAME].Web
    |-- [PROJECT-NAME].sln
    ```
- Open `settings.json` in the `.vscode` folder and set the correct project name:
    ``` json
    "eslint.workingDirectories" : [
        "./[PROJECT-NAME].Frontend",
        "./[PROJECT-NAME].Web"
    ],
    ```

### Arlanet.Frontend.Library.csproj
- Open `Arlanet.Frontend.Library.csproj` and change `<TargetFramework>netstandard2.1</TargetFramework>` to the same TargetFramework used in `[PROJECT-NAME].Web`.
- Rename `Arlanet.Frontend.Library.csproj` to `[PROJECT-NAME].Frontend.csproj`.

### svgSprite.config.json
- Add project name in `svgSprite.config.json.

### Configure CMS
  - Umbraco
    - Add correct Umbraco project name in `[PROJECT-NAME].Frontend/config/index.ts` e.g. `Arlanet.[PROJECT-NAME].Web`.
  - EpiServer
    - ToDo!

### installing dependencies
- run `npm i`.

## Configure [PROJECT-NAME].Web

### NuGet

- Install `Microsoft.AspNetCore.SpaServices.Extensions` ```dotnet add package Microsoft.AspNetCore.SpaServices.Extensions --version 7.0.7```

### Startup.cs
- Add the following code to `Startup.cs` in the configure function:
    ``` c#
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // code omitted for readability
    
        if (env.IsDevelopment())
        {
            app.UseSpa(spa =>
                spa.UseProxyToSpaDevelopmentServer("http://localhost:5174")
            );
        }
    }
    ```
  
### appsettings.Development.json

- Go to `appsettings.Development.json` and paste the following code:
    ``` json
    "Umbraco": {
        "CMS": {
            "Global": {
                "ReservedPaths": "~/app_plugins/,~/install/,~/mini-profiler-resources/,~/umbraco/,~/@vite/,~/@id/,"
            }
        }
    }
    ```
  
### Main.cshtml

- Go to `Main.cshtml` and paste the following code just before the closing body tag:
    ``` c#
    <environment include="Development">
        <script type="module" defer src="~/@@vite/client"></script>
    </environment>
    
    @foreach (var url in await SmidgeHelper.GenerateJsUrlsAsync(debug: true))
    {
        <script src="@url" type="module"></script>
    }
    ```

## package.json scripts

All the scripts can be started with the command: `npm run [script-name]`

| script-name     | runs                                                                                                            | description                                                      |
|-----------------|-----------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------|
| dev             | `vite`                                                                                                          | runs the project for development                                      |
| build           | `tsc && vite build`                                                                                             | builds the project for production                                              |
| preview         | `vite preview`                                                                                                  | broken for now...                                                |
| test:watch      | `vitest`                                                                                                        | runs vitest in watch-mode                                        |
| test:coverage   | `vitest --coverage`                                                                                             | runs vitest in watch-mode with coverage                          |
| svg:all         | `npm run svg:optimize && npm run svg:sprite`                                                                    | runs both the svg:optimize and svg:sprite scripts                |
| svg:optimize    | `svgo -f ./sprites -o ./sprites --config .svgorc.js`                                                        | [npmjs.com/svgo](https://www.npmjs.com/package/svgo)             |
| svg:sprite      | `svg-sprite sprites/*.svg --config .svgspriterc.json`                                                       | [npmjs.com/svg-sprite](https://www.npmjs.com/package/svg-sprite) |
| prettier        | `prettier --config .prettierrc.json --check --debug-check ./**/*.{js,ts,vue,scss,html}`                         | runs prettier with the --check flag                              |
| prettier:fix    | `prettier --config .prettierrc.json ./**/*.{js,ts,vue,scss,html} --write --cache --loglevel silent`             | runs prettier and fixes the problems                             |
| stylelint       | `stylelint --config .stylelintrc.json ./**/*.scss --formatter verbose`                                          | runs stylelint                                                   |
| stylelint:fix   | `stylelint --config .stylelintrc.json ./**/*.scss --formatter verbose --fix`                                    | runs stylelint and fixes the problems                            |
| eslint          | `eslint -c .eslintrc.json ./**/*.{js,ts,vue} --cache --cache-location ./dev-reports/.eslintcache`               | runs eslint                                                      |
| eslint:dry      | `eslint -c .eslintrc.json ./**/*.{js,ts,vue} --cache --cache-location ./dev-reports/.eslintcache --fix-dry-run` | runs eslint in dry-run mode                                      |
| eslint:fix      | `eslint -c .eslintrc.json ./**/*.{js,ts,vue} --cache --cache-location ./dev-reports/.eslintcache --fix`         | runs eslint and fixes the problems                               |
| storybook       | `storybook dev -p 6006`                                                                                         | runs storybook in dec mode                                       |
| storybook:build | `storybook dev -p 6006`                                                                                         | builds storybook                                                 |
| preclean        | `npm run del`                                                                                                   | runs before `clean`                                              |
| clean           | `npm install`                                                                                                   | runs `preclean` -> `clean` -> `postclean`                        |
| postclean       | `echo All node modules are re-installed! Happy coding!!`                                                        | runs after `clean`                                               |
| del             | `npx rimraf node_modules package-lock.json`                                                                     | deletes the `node_modules` and `package-lock.json`               |


