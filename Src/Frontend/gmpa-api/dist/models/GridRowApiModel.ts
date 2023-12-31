/* tslint:disable */
/* eslint-disable */
/**
 * GMPA - API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import { exists, mapValues } from '../runtime';
import type { GridColumnApiModel } from './GridColumnApiModel';
import {
    GridColumnApiModelFromJSON,
    GridColumnApiModelFromJSONTyped,
    GridColumnApiModelToJSON,
} from './GridColumnApiModel';
import type { MyRowSettings } from './MyRowSettings';
import {
    MyRowSettingsFromJSON,
    MyRowSettingsFromJSONTyped,
    MyRowSettingsToJSON,
} from './MyRowSettings';

/**
 * 
 * @export
 * @interface GridRowApiModel
 */
export interface GridRowApiModel {
    /**
     * 
     * @type {MyRowSettings}
     * @memberof GridRowApiModel
     */
    settings?: MyRowSettings;
    /**
     * 
     * @type {Array<GridColumnApiModel>}
     * @memberof GridRowApiModel
     */
    columns?: Array<GridColumnApiModel> | null;
}

/**
 * Check if a given object implements the GridRowApiModel interface.
 */
export function instanceOfGridRowApiModel(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function GridRowApiModelFromJSON(json: any): GridRowApiModel {
    return GridRowApiModelFromJSONTyped(json, false);
}

export function GridRowApiModelFromJSONTyped(json: any, ignoreDiscriminator: boolean): GridRowApiModel {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'settings': !exists(json, 'settings') ? undefined : MyRowSettingsFromJSON(json['settings']),
        'columns': !exists(json, 'columns') ? undefined : (json['columns'] === null ? null : (json['columns'] as Array<any>).map(GridColumnApiModelFromJSON)),
    };
}

export function GridRowApiModelToJSON(value?: GridRowApiModel | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'settings': MyRowSettingsToJSON(value.settings),
        'columns': value.columns === undefined ? undefined : (value.columns === null ? null : (value.columns as Array<any>).map(GridColumnApiModelToJSON)),
    };
}

